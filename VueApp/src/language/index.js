import Vue from "vue"
import VueI18n from 'vue-i18n'
import axios from 'axios'
import { messages, dateTimeFormats, numberFormats } from '@/language/enUS'
Vue.use(VueI18n);

let language = 'en-US';
const i18n = new VueI18n({
    locale: language,
    messages,
    dateTimeFormats,
    numberFormats
});

function initVueI18nDirective(obj) {
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
        bind: function (el, binding, vnode) {
            window.console.log(el.innerHTML);
            let node = getLastVnode(vnode);
            node.text = obj.$t(binding.value);
        },
        update: function (el, binding, vnode) {
            window.console.log(el.innerHTML);
            let node = getLastVnode(vnode);
            node.text = obj.$t(binding.value);
        }
    });
    Vue.directive('nf', {
        bind: function (el, binding, vnode) {
            window.console.log(vnode);
            let node = getLastNode(el);
            node.innerHTML = obj.$n(binding.value.val, binding.value.f);
        }
    });
    Vue.directive('df', {
        bind: function (el, binding, vnode) {
            window.console.log(vnode);
            let node = getLastNode(el);
            node.innerHTML = obj.$d(binding.value.val, binding.value.f);
        }
    });
}

const loadedLanguages = ['en-US'] 

function setI18nLanguage(lang) {
    i18n.locale = lang; 
    axios.defaults.headers.common['Accept-Language'] = lang;
    document.querySelector('html').setAttribute('lang', lang);
    window.localStorage.setItem('CurrentLanguage', lang);
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