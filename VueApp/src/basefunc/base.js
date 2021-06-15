function CreateBaseFunctions(vueInstance) {
    return {
        IsNullOrEmpty(str) {
            if (str === null || str === '' || str.length <= 0)
                return true;
            return false;
        },
        loginCheck: function () {
            if (vueInstance.$store.state.SessionId && ((new Date()).getTime() - vueInstance.$store.state.LastUpdateTs.getTime()) <= vueInstance.$config.sessionDuration * 1000) {
                return;
            } else {
                vueInstance.$store.state.SessionId = '';
                vueInstance.$store.state.LastUpdateTs = new Date();
                vueInstance.$router.push({ path: '/user/login', query: {} });
            }
        },
        emailValid: function (email, output) {
            let b = this.IsNullOrEmpty(email) === false;
            output.message = '';
            if (b) {
                let patt = /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
                b = patt.test(email);                
            } 
            if (!b)
                output.message = vueInstance.$t('no_email');
            return b;
        },
        pswValid: function (psw, output) {
            let b = !this.IsNullOrEmpty(psw) && (psw.length >= 8);           
            output.message = '';
            if (b) {
                b = /[a-z]+/.test(psw);
                if (b)
                    b = /[A-Z]+/.test(psw);
                if (b)
                    b = /\d+/.test(psw);
                if (b)
                    b = /\W+/.test(psw);
                
            } 
            if (!b)
                output.message = vueInstance.$t('psw_no_regulation');
            return b;
        }
    }
}
export default CreateBaseFunctions;