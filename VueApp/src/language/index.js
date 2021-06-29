import Vue from "vue"
import VueI18n from 'vue-i18n'
import axios from 'axios'
import ConstString from "@/const"
import { messages, dateTimeFormats, numberFormats } from '@/language/enUS'
Vue.use(VueI18n);

let language = 'en-US';
const i18n = new VueI18n({
    locale: language,
    messages,
    dateTimeFormats,
    numberFormats
});

function initVueI18nDirective(vueInstance) {
    function getLastNode(el) {
        let node = el;
        while (node.firstChild) {
            node = node.firstChild;
        }
        return node;
    }
    function getLastVnode(vnode) {
        let node = vnode;
        while (node.children && node.children.length > 0) {
            node = node.children[0];
        }
        return node;
    }
    Vue.directive('t', {
        bind: function (el, binding) {            
            let node = getLastNode(el);
            node.innerHTML = vueInstance.$t(binding.value);
        },
        update: function (el, binding, vnode) {           
            let node = getLastVnode(vnode);
            node.text = vueInstance.$t(binding.value);
        }
    });
    Vue.directive('tp', {
        bind: function (el, binding) {
            let node = getLastNode(el);
            node.innerHTML = vueInstance.$t(binding.value.val,binding.value.p);
        },
        update: function (el, binding, vnode) {           
            let node = getLastVnode(vnode);
            node.text = vueInstance.$t(binding.value.val, binding.value.p);
        }
    });
    Vue.directive('tc', {
        bind: function (el, binding) {
            let node = getLastNode(el);
            node.innerHTML = vueInstance.$tc(binding.value.val, binding.value.count);
        },
        update: function (el, binding, vnode) {           
            let node = getLastVnode(vnode);
            node.text = vueInstance.$tc(binding.value.val, binding.value.count);
        }
    });
    Vue.directive('nf', {
        bind: function (el, binding) {
            let node = getLastNode(el);
            node.innerHTML = vueInstance.$n(binding.value.val, binding.value.f);
        },
        update: function (el, binding, vnode) {            
            let node = getLastVnode(vnode);
            node.text = vueInstance.$n(binding.value.val, binding.value.f);
        }
    });
    Vue.directive('df', {
        bind: function (el, binding) {
            let node = getLastNode(el);
            node.innerHTML = vueInstance.$d(binding.value.val, binding.value.f);
        },
        update: function (el, binding, vnode) {           
            let node = getLastVnode(vnode);
            node.text = vueInstance.$d(binding.value.val, binding.value.f);
        }
    });
}

const loadedLanguages = ['en-US'] 

function setI18nLanguage(lang) {
    i18n.locale = lang; 
    axios.defaults.headers.common['Accept-Language'] = lang;
    document.querySelector('html').setAttribute('lang', lang);
    window.localStorage.setItem(ConstString.CurrentLanguage, lang);
    return lang
}

function loadLanguageAsync(lang) {    
    if (i18n.locale === lang) {
        return Promise.resolve(setI18nLanguage(lang))
    }        
    if (loadedLanguages.includes(lang)) {
        return Promise.resolve(setI18nLanguage(lang))
    }    
    let langFolder = lang.replace('-', '');
    return import(`@/language/${langFolder}`).then(
        language => {
            i18n.setLocaleMessage(lang, language.messages[lang]);
            i18n.setDateTimeFormat(lang, language.dateTimeFormats[lang]);
            i18n.setNumberFormat(lang, language.numberFormats[lang]);
            loadedLanguages.push(lang);
            return setI18nLanguage(lang);
        }
    )
}
export { i18n, initVueI18nDirective, loadLanguageAsync}