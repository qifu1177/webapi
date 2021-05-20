<template>
    <div>
        <b-input-group>
            <b-form-input v-model="selectedFiles.str" @click="openFileManager()" :readonly="true"></b-form-input>            
        </b-input-group>
        <div v-if="showFileManager">
            <b-card>
                <FileManage v-bind:more-selected="moreSelected" v-bind:selected-files="selectedFileList"></FileManage>
                <br />
                <b-button-group size="sm" class="mx-3">
                    <b-button @click="btOkClick()">OK</b-button>
                    <b-button @click="showFileManager=false">Cancel</b-button>
                </b-button-group>
            </b-card>
        </div>
    </div>
</template>

<script>
    import fileManager from '@/components/FileManage.vue';
    export default {
        name: 'FileSelect',
        components: {
            FileManage: fileManager
        },
        props: {
            selectedFiles: {
                str: ''
            },
            moreSelected: Boolean
        },
        computed: {

        },
        data: function () {
            return {
                showFileManager: false,
                selectedFileList: []
            }
        },
        watch: {
        },
        methods: {
            btOkClick() {
                this.selectedFiles.str = this.selectedFileList.join();
                this.showFileManager = false;
            },
            openFileManager() {
                this.selectedFileList.splice(0, this.selectedFileList.length);
                if (this.selectedFiles.str)
                    this.selectedFileList = this.selectedFiles.str.split(',');
                this.showFileManager = true;
            }
        }
    };
</script>