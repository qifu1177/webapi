<template>
    <div class="container-fluid">
        <div class="row">
            <b-button-toolbar aria-label="Toolbar">
                <b-button-group size="sm" class="mx-3">
                    <b-button @click="showList=true">List</b-button>
                    <b-button @click="showList=false">Card</b-button>
                </b-button-group>
            </b-button-toolbar>
        </div>
        <div v-if="showList">
            <b-table hover :items="fileList" :fields="fields" show-empty>
                <template #cell(name)="row">
                    <div v-if="row.item.isImage" v-on:dblclick="openImage(row.item.name,row.item.link)" variant="light">{{row.item.name}}</div>
                    <a v-else target="_blank" v-bind:href="row.item.link">{{row.item.name}}</a>
                </template>
                <template #cell(fileType)="row">
                    <div>{{row.item.fileType}}</div>
                </template>
                <template #cell(size)="row">
                    <div>{{sizeToMb(row.item.size)}}</div>
                </template>

                <template #cell(actions)="row">
                    <b-button-group>
                        <b-button variant="light" size="sm" @click="deleteFile(row.item, $event.target)">
                            <b-icon icon="trash" variant="danger"></b-icon>
                        </b-button>
                    </b-button-group>
                </template>
            </b-table>
        </div>
        <div v-else>

        </div>
        <div>
            <b-form-file v-model="selectedUploadFiles"
                         :state="uploadFile(selectedUploadFiles)"
                         placeholder="Choose a file or drop it here..."
                         drop-placeholder="Drop file here..." 
                         :readonly="inputFileReadonly"></b-form-file>
            <div class="row" v-show="showUploadState">
                <div class="col">
                    {{selectedUploadFiles.name}}
                </div>
                <div class="col">
                    <b-progress :value="uploadbackdata.value" :max="100" class="mb-3"></b-progress>
                    <b-alert v-model="showUploadFileAlert" variant="danger" dismissible>
                        {{uploadbackdata.message}}
                    </b-alert>
                </div>
                <div class="col">
                    <b-button variant="primary" :readonly="reuploadReadonly" @click="uploadFile(selectedUploadFiles)">try again</b-button>
                </div>
            </div>

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
        data: function () {
            return {
                showList: true,
                selectedUploadFiles: [],
                fileList: [],
                fields: [
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
                uploadbackdata: { value: 0, uploadFileName:'', message: '', exception: '', state: window.$UploadFileState.free },
                outputData: { message: '', exception: '' }
            }
        },
        computed: {
            showUploadFileAlert() {
                return this.uploadbackdata.state == window.$UploadFileState.fail;
            },
            inputFileReadonly() {
                return this.uploadbackdata.state == window.$UploadFileState.run;
            },
            reuploadReadonly() {
                return this.uploadbackdata.state != window.$UploadFileState.fail;
            },
            showUploadState() {
                return this.uploadbackdata.state == window.$UploadFileState.fail || this.uploadbackdata.state == window.$UploadFileState.run;
            }

        },
        methods: {
            sizeToMb(bytevalue) {
                return bytevalue / 1000;
            },
            loadSessionId() {
                logic.loadSessionId(this.session);
            },
            loadFiles() {
                this.fileList = [];
                logic.loadFiles(this.fileList);
            },
            uploadFile(uploadFile) {
                if (uploadFile && uploadFile.name &&
                    (uploadFile.name != this.uploadbackdata.uploadFileName || this.uploadbackdata.state == window.$UploadFileState.fail)) {
                    this.uploadbackdata.value = 0;
                    this.uploadbackdata.message = '';
                    this.uploadbackdata.state = window.$UploadFileState.run;
                    this.uploadbackdata.uploadFileName = uploadFile.name;
                    logic.uploadFile(this.session.id, uploadFile, this.uploadbackdata);
                    this.updateUploadState();
                }
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
            openImage(name, link) {
                window.console.log(name);
                window.console.log(link);
            },
            deleteFile: function (item, button) {
                window.console.log(button);
                logic.deleteFile(item.name, this.outputData);
                this.loadFiles();
            },
        },
        beforeMount() {
            logic.init(this.$baseServerUrl);
            this.loadSessionId();
            this.loadFiles();
        },
    };
</script>