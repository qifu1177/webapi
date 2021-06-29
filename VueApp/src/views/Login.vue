<template>
    <b-container fluid>
        <br />
        <b-row>
            <b-col>
                <b-form-input ref="emailInput" v-model="form.email" v-bind:placeholder="$t('input_email')" :type="'email'" v-focus></b-form-input>
            </b-col>
        </b-row>
        <br />
        <b-row>
            <b-col>
                <b-form-input v-model="form.psw" v-bind:placeholder="$t('input_psw')" :type="'password'" v-enter="login"></b-form-input>
            </b-col>
        </b-row>
        <b-row v-if="showMessage">
            <b-col>
                <p class="error-message" v-t="output.data.message"></p>
            </b-col>
        </b-row>
        <br />
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
            showMessage() {
                if (this.output.data.message != '' && this.output.data.message != 'OK')
                    return true;
                return false;
            }
        },
        watch: {
            output: {
                handler(newData) {
                    if (newData.data.message && newData.data.message == 'OK') {                      
                        this.goBack();
                    }
                },
                deep: true
            }
        },
        methods: {
            //onKeyup(event) {
            //    if (event.keyCode === 13)
            //        this.login();
            //},
            login() {
                this.output.data.message = '';
                userLogic.userLogin(this.form, this.output);
            },
            register() {
                this.$router.push({ path: '/user/registe', query: {} });
            },
            goBack() {               
                this.$store.commit("authenticate/Login", {
                    data: this.output.data,
                    LoginInfo: this.$const.LoginInfo,
                    afterLogin: () => {
                        this.$store.dispatch("authenticate/Logout", {
                            duration: this.$config.session.duration,
                            Logout: () => {
                                this.$router.push({ path: '/user/login', query: {} });
                            },
                            LoginInfo: this.$const.LoginInfo
                        });
                        this.$router.go(-1);
                    }
                });                
            },
            updateDataFromSessionStory() {
                let loginInfo = window.sessionStorage.getItem(this.$const.LoginInfo);
                if (loginInfo) {
                    let data = JSON.parse(loginInfo);
                    if (((new Date()).getTime() - data.loginTs) <= this.$config.session.duration * 1000) {
                        this.output.data = data;
                    }
                }
            }
        },
        mounted() {            
            //let input = this.$refs.emailInput;
            //input.focus();            
        },
        beforeMount() {
            this.updateDataFromSessionStory();
            userLogic.init(this.$config.baseServerUrl);
            userLogic.setErrorFunc(this.$createShowMessage('error', this));
        }        
    }
</script>