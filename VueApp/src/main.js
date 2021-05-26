import Vue from 'vue';
import router from './router'
import { BootstrapVue, IconsPlugin } from 'bootstrap-vue'
import { i18n, initVueI18nDirective, loadLanguageAsync } from '@/language'
import App from './App.vue';

import 'bootstrap/dist/css/bootstrap.css'
import 'bootstrap-vue/dist/bootstrap-vue.css'
import './app.css'

Vue.use(BootstrapVue)
Vue.use(IconsPlugin)
Vue.config.productionTip = true;
Vue.prototype.$baseServerUrl = 'https://localhost:44356';
Vue.prototype.$loadLanguageAsync = loadLanguageAsync;
Vue.prototype.$createShowMessage = (type, vueComponent) => {
    let variant = 'default';
    switch (type) {
        case 'error':
            variant = 'danger';
            break;
        case 'warning':
            variant = 'warning';
            break;
        case 'success':
            variant = 'success';
            break;
        case 'info':
            variant = 'info';
            break;
        default:
            variant = 'default';
    }
    return function (message) {
        let text = '';
        if (typeof message === 'string') {
            text = message;
        } else if (typeof message === 'object' && message.message != null) {
            text = message.message;
        }
        vueComponent.$bvToast.toast(text, {
            title: '',
            variant: variant,
            solid: true,
            noAutoHide: true
        });
    };    
};
let vue=new Vue({  
    render: h => h(App),
    router,   
    i18n, 
    BootstrapVue,
    IconsPlugin
});
initVueI18nDirective(vue);
vue.$mount('#app');
