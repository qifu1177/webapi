<template>

    <b-container fluid v-if="showForm">
        <br />
        <b-form-group :invalid-feedback="error.email.message" :state="error.email.state">
            <b-form-input ref="emailInput" v-model="form.email" v-bind:placeholder="$t('input_email')" :type="'email'" :state="error.email.state" trim v-focus></b-form-input>
        </b-form-group>
        <b-form-group :invalid-feedback="error.psw.message" :state="error.psw.state">
            <b-form-input v-model="form.psw" v-bind:placeholder="$t('input_psw')" :type="'password'" :state="error.psw.state" trim></b-form-input>
        </b-form-group>
        <b-form-group :invalid-feedback="error.repeatPsw.message" :state="error.repeatPsw.state">
            <b-form-input v-model="form.repeatPsw" v-bind:placeholder="$t('input_repsw')" :type="'password'" :state="error.repeatPsw.state" trim></b-form-input>
        </b-form-group>
        <b-form-group :invalid-feedback="error.userName.message" :state="error.userName.state">
            <b-form-input v-model="form.userName" v-bind:placeholder="$t('input_username')" :type="'text'" :state="error.userName.state" trim></b-form-input>
        </b-form-group>

        <b-form-group :invalid-feedback="error.aczept.message" :state="error.aczept.state">
            <b-button v-t="'show_declaration'" @click="showDeclaration()" variant="light"></b-button>
        </b-form-group>
        <b-row>
            <b-col>
                <b-button-group>
                    <b-button v-t="'register'" @click="register()" variant="success"></b-button>
                    <b-button v-t="'back'" @click="goBack()" variant="light"></b-button>
                </b-button-group>
            </b-col>
        </b-row>
    </b-container>
    <b-container fluid v-else>
        <p v-t="'registerSuccessInfo'"></p>
        <b-button v-t="'toHome'" @click="goToHome()" variant="success"></b-button>
    </b-container>
</template>
<script>
    import userLogic from "@/logics/UserLogic"
    export default {
        name: 'Regeister',
        data: function () {
            return {
                form: {
                    email: '',
                    psw: '',
                    repeatPsw: '',
                    userName: '',
                },
                error: {
                    email: { message: '', state: null },
                    psw: { message: '', state: null },
                    repeatPsw: { message: '', state: null },
                    userName: { message: '', state: null },
                    aczept: { message: '', state: null }
                },
                output: {
                    data: {
                        message: ''
                    }
                },
                showForm: true
            }
        },
        computed: {
        },
        watch: {
            output: {
                handler(newData) {
                    if (newData.data.message && newData.message == 'OK') {
                        this.output.data.message = '';
                        this.showSuccess();
                    }
                },
                deep: true
            }
        },
        methods: {
            test() {
                window.console.log('a+b');
            },
            goBack() {
                this.$router.go(-1);
            },
            repeatPswState() {
                let b = this.form.psw == this.form.repeatPsw;
                this.setMessage(this.error.repeatPsw, '');
                if (!b)
                    this.setMessage(this.error.repeatPsw, this.$t('repeatPsw_error'));
                return b;
            },
            userNameState() {
                this.setMessage(this.error.userName, '');
                let b = this.$base.IsNullOrEmpty(this.form.userName);
                if (b) {
                    this.setMessage(this.error.userName, this.$t('userName_error'));
                }
                return !b;
            },
            valid() {
                this.error.email.state = this.$base.emailValid(this.form.email, this.error.email);
                this.error.psw.state = this.$base.pswValid(this.form.psw, this.error.psw);
                this.error.repeatPsw.state = this.repeatPswState();
                this.error.userName.state = this.userNameState();
                if (this.error.aczept.state === null) {
                    this.error.aczept.state = false;
                    this.error.aczept.message = this.$t('declaration_no_aczept');
                }
                let state = true;
                for (let key in this.error) {
                    state &&= this.error[key].state;
                }
                return state;
            },
            register() {
                if (this.valid()) {
                    userLogic.userRegister({ email: this.form.email, psw: this.form.psw, userName: this.form.userName }, this.output);
                }
            },
            setMessage(obj, message) {
                obj.message = message;
            },
            showDeclaration() {
                const id = 'toast_declaration_div_1';
                const h = this.$createElement;
                const $div = h(
                    'div',
                    {
                    },
                    [
                        h('p',
                            {
                            }, this.$t('declaration_info')),
                        h('b-button-group', {}, [
                            h('b-button', { attrs: { variant: 'success' }, on: { click: () => this.declarationOk('toast_declaration_div_1') } }, this.$t('ok')),
                            h('b-button', { attrs: { variant: 'light' }, on: { click: () => this.declarationCancel('toast_declaration_div_1') } }, this.$t('cancel'))
                        ])
                    ]

                );
                this.$bvToast.toast([$div], {
                    id: id,
                    title: this.$t('title_declaration'),
                    toaster: 'b-toaster-top-full',
                    solid: true,
                    appendToast: true
                })
            },
            declarationOk(id) {
                this.error.aczept.state = true;
                this.error.aczept.message = '';
                this.$bvToast.hide(id);
            },
            declarationCancel(id) {
                this.error.aczept.state = false;
                this.error.aczept.message = this.$t('declaration_no_aczept');
                this.$bvToast.hide(id);
            },
            showSuccess() {
                this.showForm = false;
            },
            goToHome() {

            }
        },
        mounted() {
            //let input = this.$refs.emailInput;
            //input.focus();
        },
        beforeMount() {
            userLogic.init(this.$config.baseServerUrl);
            userLogic.setErrorFunc(this.$createShowMessage('error', this));
        }
    }
</script>