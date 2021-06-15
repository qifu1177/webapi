import Vue from 'vue';
import Vuex from 'vuex';
Vue.use(Vuex);
const store = new Vuex.Store({
    state: {
        SessionId: '',
        LastUpdateTs: new Date()
    }
});
export default store;