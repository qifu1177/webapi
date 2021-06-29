import Vue from 'vue';
import Vuex from 'vuex';
import authenticate from './authenticate'
Vue.use(Vuex);
const store = new Vuex.Store({
    modules: {
        authenticate: authenticate
    }
});
export default store;