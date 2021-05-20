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
                    <div @click="openFile(row.item)" variant="light" style="cursor:pointer;">{{row.item.name}}</div>
                </template>
                <template #cell(fileType)="row">
                    <div>{{row.item.fileType}}</div>
                </template>
                <template #cell(size)="row">
                    <div>{{sizeToMb(row.item.size)}}</div>
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
                            <img v-bind:src="item.icon" v-bind:alt="item.fileType" class="img-small" @click="openFile(item)"/>
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

    export default {
        name: 'FileManage',
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
                showImg: { src: '', title: '' }
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
            loadSessionId() {
                logic.loadSessionId(this.session);
            },
            loadFiles() {
                this.fileList = [];
                logic.loadFiles(this.fileList, this.selectedFiles);
            },
            uploadFile(uploadFile) {
                if (uploadFile && uploadFile.name &&
                    uploadFile.name != this.uploadbackdata.uploadFileName) {
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

            toast(item, toaster, append = false) {
                const h = this.$createElement;
                const $img = h(
                    'div',
                    {
                        class: 'div-img'
                    },
                    [
                        h('b-img',
                            {
                                class: 'img-fluid',
                                attrs: { src: item.link, alt: item.name }
                            }, '')
                    ]

                );
                this.$bvToast.toast([$img], {
                    title: `${item.name}`,
                    toaster: toaster,
                    solid: true,
                    appendToast: append,
                    noAutoHide: true
                })
            },
            openFile(item) {
                if (item.isImage) {
                    this.showImg.src = item.link;
                    this.showImg.title = item.name;
                    this.toast(item, "b-toaster-top-full", true);
                } else
                    window.open(item.link, "_blank");
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
            this.loadSessionId();
            this.loadFiles();
        },
    };
</script>