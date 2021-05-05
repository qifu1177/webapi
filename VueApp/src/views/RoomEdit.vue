<template>
    <div class="container-fluid">
        <b-form @submit="onSubmit" @reset="onReset" v-if="show">

            <b-form-group label="Name:" label-for="Name">
                <b-form-input v-model="form.Name"
                              placeholder="Enter name"
                              required></b-form-input>
            </b-form-group>
            <b-form-group label="Image:" label-for="Image">
                <b-form-file v-model="selectedFiles"
                             :state="uploadFile()"
                             placeholder="Choose a file or drop it here..." accept=".pdf"
                             drop-placeholder="Drop file here..."></b-form-file>
            </b-form-group>
            <b-button-group size="sm" class="mx-3">
                <b-button type="submit" variant="primary">Submit</b-button>
                <b-button type="reset" variant="danger">Reset</b-button>
                <b-button variant="danger" @click="back()">back</b-button>
            </b-button-group>

        </b-form>
    </div>
</template>
<script>
    import roomLogic from '@/logics'
    roomLogic.init('https://localhost:44356/data');
    export default {
        name: 'Room',
        data: function () {
            return {
                form: {
                },
                selectedFiles: [],
                show: true,
                session: { id: '' },
                uploadbackdata: { message: '', exception: '' },
                outputData: { message: '', exception: '' }
            }
        },
        computed: {

        },
        mounted: function () {
            this.form = this.clone(this.$router.currentRoute.query);
        },
        methods: {
            onSubmit(event) {
                event.preventDefault();

                if (this.selectedFiles.length > 0)
                    this.form.ImagePath = this.selectedFiles[0].name;
                this.form.TS = new Date().getTime();
                roomLogic.saveRoom(this.session.id, this.form, this.outputData);
            },
            onReset(event) {
                event.preventDefault();
                this.form = this.clone(this.$router.currentRoute.query);
            },
            back() {
                this.$router.go(-1);
            },
            clone(source) {
                let obj = {};
                for (let k in source)
                    obj[k] = source[k];
                return obj;
            },
            loadSessionId() {
                roomLogic.loadSessionId(this.session);
            },
            uploadFile() {
                //debugger;
                if (this.selectedFiles.length > 0) {
                    roomLogic.uploadFile(this.session.id, this.selectedFiles[0], this.uploadbackdata);
                }
            }
        },
        beforeMount() {
            this.loadSessionId();
            this.form = this.clone(this.$router.currentRoute.query);
        },
    };
</script>