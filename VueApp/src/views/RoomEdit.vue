<template>
    <div class="container-fluid">
        <b-form @submit="onSubmit" @reset="onReset" v-if="show">

            <b-form-group label="Name:" label-for="Name">
                <b-form-input v-model="form.Name"
                              placeholder="Enter name"
                              required></b-form-input>
            </b-form-group>
           
            <b-button-group size="sm" class="mx-3">
                <b-button type="submit" variant="primary">Submit</b-button>
                <b-button type="reset" variant="danger">Reset</b-button>
                <b-button variant="danger" @click="back()">back</b-button>
            </b-button-group>

        </b-form>
        <FileManage  v-bind:more-selected="false" v-bind:selected-files="selectedFiles"></FileManage>
    </div>
</template>
<script>
    import roomLogic from '@/logics';
    import fileManager from '@/components/FileManage.vue';
    
    export default {
        name: 'Room',
        components: {
            FileManage: fileManager
        },
        data: function () {
            return {
                form: {
                },                
                show: true,
                selectedFiles:[],
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
            }
            
        },
        beforeMount() {
            roomLogic.init(this.$baseServerUrl);
            this.loadSessionId();
            this.form = this.clone(this.$router.currentRoute.query);
        },
    };
</script>