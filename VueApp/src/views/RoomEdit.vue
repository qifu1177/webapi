<template>
    <div class="container-fluid">
        <b-form @submit="onSubmit" @reset="onReset" v-if="show">

            <b-form-group label="Name:" label-for="Name">
                <b-form-input v-model="form.Name"
                              placeholder="Enter name"
                              required></b-form-input>
            </b-form-group>
            <b-form-group label="Image:" label-for="ImagePath">                
                <FileSelect v-bind:selectedFiles="selectedFile" v-bind:moreSelected="false" ></FileSelect>
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
    import roomLogic from '@/logics';
    import fileSelect from '@/components/FileSelect.vue';
    
    export default {
        name: 'Room',
        components: {
            FileSelect: fileSelect
        },
        data: function () {
            return {
                form: {
                },                
                show: true,          
                selectedFile: {name:''},
                session: { id: '' },
                uploadbackdata: { message: '', exception: '' },
                outputData: { message: '', exception: '' }
            }
        },
        computed: {
            
        },
        watch: {
            outputData: {
                handler(newData) {
                    if (newData.message && newData.message == 'OK') {
                        this.outputData.message = '';
                        this.$router.go(-1);
                    }
                },
                deep: true
            }
        },
        mounted: function () {
            this.form = this.clone(this.$router.currentRoute.query);
            this.selectedFile.name = this.form.ImagePath;
        },
        methods: {
            onSubmit(event) {
                event.preventDefault();                
                this.form.TS = new Date().getTime();
                this.form.ImagePath = this.selectedFile.name;
                roomLogic.saveRoom(this.session.id, this.form, this.outputData);
            },
            onReset(event) {
                event.preventDefault();
                this.form = this.clone(this.$router.currentRoute.query);   
                this.selectedFile.name = this.form.ImagePath;
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
            roomLogic.init(this.$config.baseServerUrl);
            roomLogic.setErrorFunc(this.$createShowMessage('error', this));
            this.loadSessionId();           
        }
       
    };
</script>