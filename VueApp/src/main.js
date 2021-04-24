import Vue from 'vue';
import axios from 'axios'
import router from './router'
import { BootstrapVue, IconsPlugin } from 'bootstrap-vue'
import App from './App.vue';

import 'bootstrap/dist/css/bootstrap.css'
import 'bootstrap-vue/dist/bootstrap-vue.css'

Vue.use(BootstrapVue)
Vue.use(IconsPlugin)
Vue.config.productionTip = true;

Vue.prototype.$http = axios;

new Vue({  
    render: h => h(App),
    router,
    BootstrapVue,
    IconsPlugin
}).$mount('#app');
