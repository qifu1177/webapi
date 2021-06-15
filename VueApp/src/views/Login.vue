<template>
    <b-container fluid>
        <br />
        <b-row>
            <b-col>
                <b-form-input ref="emailInput" v-model="form.email" v-bind:placeholder="$t('input_email')" :type="'email'"></b-form-input>
            </b-col>
        </b-row>
        <b-row>
            <b-col>
                <b-form-input v-model="form.psw" v-bind:placeholder="$t('input_psw')" :type="'password'"></b-form-input>
            </b-col>
        </b-row>
        <b-row>
            <b-col>
                <b-button-group>
                    <b-button v-t="'login'" @click="login()" variant="success"></b-button>
                    <b-button v-t="'register'" @click="register()" variant="light"></b-button>

                </b-button-group>
            </b-col>
        </b-row>
    </b-container>
</template>
<script>
    import userLogic from "@/logics/UserLogic"
    export default {
        name: 'Login',
        data: function () {
            return {
                form: {
                    email: '',
                    psw: ''
                },
                output: {
                    data: {
                        message: ''
                    }
                }
            }
        },
        computed: {
        },
        watch: {
            output: {
                handler(newData) {
                    if (newData.data.message && newData.data.message == 'OK') {
                        this.output.data.message = '';
                        this.goBack();
                    }
                },
                deep: true
            }
        },
        methods: {
            login() {
                userLogic.userLogin(this.form, this.output);
            },
            register() {
                this.$router.push({ path: '/user/registe', query: {} });
            },
            goBack() {
                this.$store.state.LastUpdateTs = new Date(this.output.data.loginTs);
                this.$store.state.SessionId = this.output.data.sessionId;
                this.$router.go(-1);
            }
        },
        mounted() {
            let loginInfo = window.sessionStorage.getItem(this.$const.LoginInfo);
            if (loginInfo) {
                let data = JSON.parse(loginInfo);
                if (((new Date()).getTime() - data.loginTs) <= this.$config.sessionDuration * 1000) {
                    this.output.data = data;
                }
            }
            let input = this.$refs.emailInput;
            input.focus();            
        },
        beforeMount() {
            userLogic.init(this.$config.baseServerUrl);
            userLogic.setErrorFunc(this.$createShowMessage('error', this));
        }
    }
</script>