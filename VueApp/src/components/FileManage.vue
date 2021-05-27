<template>
    <div class="container-fluid">
        <div class="row">
            <b-button-toolbar aria-label="Toolbar">
                <b-button-group size="sm" class="mx-3">
                    <b-button @click="showList=true"><b-icon icon="list-check"></b-icon></b-button>
                    <b-button @click="showList=false"><b-icon icon="grid"></b-icon></b-button>
                </b-button-group>
            </b-button-toolbar>
        </div>
        <br />
        <div>
            <div>
                <b-form-file v-model="selectedUploadFiles"
                             :state="uploadFile(selectedUploadFiles)"
                             placeholder="Choose a file or drop it here..."
                             drop-placeholder="Drop file here..."
                             v-bind:readonly="inputFileReadonly"></b-form-file>
            </div>
            <p>
                Upload file type: {{fileSetting.fileTypes}}; maximum size: {{fileSetting.maxSize}} MB.
            </p>
            <br />
            <div class="row" v-show="showUploadState">
                <div class="col">
                    {{selectedUploadFiles.name}}
                </div>
                <div class="col">
                    <b-progress :value="uploadbackdata.value" :max="100" class="mb-3"></b-progress>
                </div>
                <div class="col">
                    <b-button variant="primary" v-bind:readonly="reuploadReadonly" @click="reuploadFile()">try again</b-button>
                </div>
            </div>
        </div>
        <b-input-group>
            <b-form-input v-model="searchStr"></b-form-input>
            <b-input-group-prepend is-text>
                <b-icon icon="search" variant="secondary"></b-icon>
            </b-input-group-prepend>
        </b-input-group>
        <div v-if="showList">
            <b-table hover :items="searchResults" :fields="fields" show-empty>
                <template #cell(selected)="row">
                    <b-form-checkbox v-model="row.item.selected" @change="selectedChanged(row.item)">
                    </b-form-checkbox>
                </template>
                <template #cell(name)="row">                   
                    <FileView :is-list="true" :is-img="row.item.isImage" :name="row.item.name" :icon="row.item.icon" :link="row.item.link"></FileView>
                </template>
                <template #cell(fileType)="row">
                    <div>{{row.item.fileType}}</div>
                </template>
                <template #cell(size)="row" >
                    <div v-nf="{'val':sizeToMb(row.item.size)}"></div>
                </template>

                <template #cell(actions)="row">
                    <b-button-group>
                        <b-button variant="light" size="sm" @click="row.toggleDetails">
                            <b-icon icon="trash" variant="danger"></b-icon>
                        </b-button>
                    </b-button-group>
                </template>
                <template #row-details="row">
                    <b-card>
                        <div>
                            Are you sure you want to delete the file {{row.item.name}}?
                        </div>
                        <b-button-group size="sm" class="mx-3">
                            <b-button size="sm" @click="deleteFile(row.item, $event.target)" variant="danger">OK</b-button>
                            <b-button size="sm" @click="row.toggleDetails" variant="light">Cancel</b-button>
                        </b-button-group>
                    </b-card>
                </template>
            </b-table>
        </div>
        <div v-else>
            <b-card-group columns>
                <b-card v-for="(item,index) in searchResults" :key="index">
                    <b-row>
                        <b-col align="center">                           
                            <FileView :is-list="false" :is-img="item.isImage" :name="item.name" :icon="item.icon" :link="item.link"></FileView>
                        </b-col>
                    </b-row>
                    <b-row>
                        <b-col align="center"><b>{{item.name}}</b></b-col>
                    </b-row>
                    <b-row>
                        <b-col align="center">
                            <b-button-group size="sm" class="mx-3">
                                <b-form-checkbox v-model="item.selected" @change="selectedChanged(item)">
                                </b-form-checkbox>
                                <b-button variant="light" size="sm" @click="item.showDelete=true">
                                    <b-icon icon="trash" variant="danger"></b-icon>
                                </b-button>
                            </b-button-group>
                        </b-col>
                    </b-row>
                    <b-row v-show="item.showDelete">
                        <b-col>
                            <div>
                                Are you sure you want to delete the file ?
                            </div>
                            <b-button-group size="sm" class="mx-3">
                                <b-button size="sm" @click="deleteFile(item, $event.target)" variant="danger">OK</b-button>
                                <b-button size="sm" @click="item.showDelete=false" variant="light">Cancel</b-button>
                            </b-button-group>
                        </b-col>
                    </b-row>
                </b-card>
            </b-card-group>
        </div>
    </div>
</template>
<script>
    import logic from '@/logics/Logic'
    import fileView from '@/components/FileView.vue';
    export default {
        name: 'FileManage',
        components: {
            FileView: fileView
        },
        props: {
            selectedFiles: Array,
            moreSelected: Boolean
        },
        computed: {
            inputFileReadonly() {
                return this.uploadbackdata.state == window.$UploadFileState.run;
            },
            reuploadReadonly() {
                return this.uploadbackdata.state != window.$UploadFileState.fail;
            },
            showUploadState() {
                return this.uploadbackdata.state == window.$UploadFileState.fail || this.uploadbackdata.state == window.$UploadFileState.run;
            },
            searchResults() {
                let results = [];
                for (let i = 0; i < this.fileList.length; i++) {
                    if (this.fileList[i].name.toLowerCase().indexOf(this.searchStr.toLowerCase()) >= 0)
                        results.push(this.fileList[i]);
                }
                return results;
            }
        },
        data: function () {
            return {
                showList: true,
                selectedUploadFiles: [],
                fileList: [],
                fields: [
                    {
                        key: 'selected',
                        sortable: true
                    },
                    {
                        key: 'name',
                        sortable: true
                    },
                    {
                        key: 'fileType',
                        sortable: true
                    },
                    {
                        key: 'size',
                        sortable: true,
                        label: 'Size [KB]'
                    },
                    { key: 'actions', label: 'Actions' }

                ],
                session: { id: '' },
                uploadbackdata: { value: 0, uploadFileName: '', message: '', exception: '', state: window.$UploadFileState.free },
                outputData: { message: '', exception: '' },
                searchStr: '',
                fileSetting: {fileTypes:"",maxSize:0}
                
            }
        },
        watch: {
            outputData: {
                handler(newData) {
                    if (newData.message && newData.message == 'OK') {
                        this.outputData.message = '';
                        this.loadFiles();
                    }
                },
                deep: true
            }
        },
        methods: {
            sizeToMb(bytevalue) {
                return bytevalue / 1000;
            },
            selectedChanged(item) {
                if (!this.moreSelected) {
                    for (let i = 0; i < this.fileList.length; i++) {
                        if (this.fileList[i].name != item.name)
                            this.fileList[i].selected = false;
                    }
                }
                this.updateSelectedFiles();
            },
            updateSelectedFiles() {
                this.selectedFiles.splice(0, this.selectedFiles.length);
                for (let i = 0; i < this.fileList.length; i++) {
                    if (this.fileList[i].selected)
                        this.selectedFiles.push(this.fileList[i].name);
                }
            },
            loadFileSetting() {
                logic.loadFileSetting(this.fileSetting);
            },
            loadSessionId() {
                logic.loadSessionId(this.session);
            },
            loadFiles() {
                this.fileList = [];
                logic.loadFiles(this.fileList, this.selectedFiles);
            },
            validFile(selectedFile) {
                if (selectedFile && selectedFile.name &&
                    selectedFile.name != this.uploadbackdata.uploadFileName) {
                    let showErrorMessage = this.$createShowMessage('error', this);
                    var patt = eval('/(' + this.fileSetting.fileTypes+')$/g');
                    var res = patt.test(selectedFile.name);
                    if (!res) {
                        showErrorMessage('The type of the uploaded file is invalid.');
                        return false;
                    }
                    let maxSize = parseInt(this.fileSetting.maxSize) * 1024 * 1024;
                    if (maxSize < selectedFile.size) {
                        showErrorMessage('The uploaded file is too large.');
                        return false;
                    }
                    return true;
                }
                return false;
            },
            uploadFile(uploadFile) {
                if (this.validFile(uploadFile)) {
                    this.uploadbackdata.value = 0;
                    this.uploadbackdata.message = '';
                    this.uploadbackdata.state = window.$UploadFileState.run;
                    this.uploadbackdata.uploadFileName = uploadFile.name;
                    logic.uploadFile(this.session.id, uploadFile, this.uploadbackdata);
                    this.updateUploadState();
                }
            },
            reuploadFile() {
                this.uploadbackdata.uploadFileName = '';
                this.uploadFile(this.selectedUploadFiles);
            },
            updateUploadState() {
                let that = this;
                window.setTimeout(() => {
                    if (that.uploadbackdata.state == window.$UploadFileState.successful) {
                        that.uploadbackdata.value = 100;
                        that.uploadbackdata.message = '';
                        that.uploadbackdata.state = window.$UploadFileState.free;
                        that.loadFiles();
                    }
                    if (that.uploadbackdata.state == window.$UploadFileState.fail)
                        that.uploadbackdata.value = 0;
                    if (that.uploadbackdata.state == window.$UploadFileState.successful || that.uploadbackdata.state == window.$UploadFileState.fail)
                        return;
                    if (that.uploadbackdata.value < 100)
                        that.uploadbackdata.value = that.uploadbackdata.value + 10;
                    that.updateUploadState();
                }, 50);
            },
                       
            deleteFile: function (item, button) {
                window.console.log(button);
                this.outputData.message = '';
                logic.deleteFile(item.name, this.outputData);
            }
        },
        beforeMount() {
            logic.init(this.$baseServerUrl);
            logic.setErrorFunc(this.$createShowMessage('error', this));
            this.loadFileSetting();
            this.loadSessionId();
            this.loadFiles();
        },
    };
</script>