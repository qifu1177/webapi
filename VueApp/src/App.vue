<template>
    <div id="app">
        <div>
            <b-navbar toggleable="lg" type="dark" variant="info">
                <b-navbar-toggle target="nav-collapse"></b-navbar-toggle>
                <b-collapse id="nav-collapse" is-nav>
                    <b-navbar-nav>
                        <b-nav-item to="/" v-t="'home'"></b-nav-item>
                        <b-nav-item to="/room" v-t="'room'"></b-nav-item>
                        <b-nav-item to="/cupboard">Cupboard</b-nav-item>
                        <b-nav-item to="/thing">Thing</b-nav-item>
                    </b-navbar-nav>

                    <!-- Right aligned nav items -->
                    <b-navbar-nav class="ml-auto">
                        <b-nav-form>
                            <b-input-group>
                                <b-form-input placeholder="Search" @focus="showSelectView()"></b-form-input>
                                <b-input-group-prepend is-text variant="light"><b-icon variant="success" icon="search"></b-icon></b-input-group-prepend>
                            </b-input-group>
                        </b-nav-form>

                        <b-nav-item-dropdown v-bind:text="currentLanguage.name" right>
                            <b-dropdown-item v-for="(item,index) in showLanguageList" :key="index" @click="changeLanguage(item.val)">{{item.name}}</b-dropdown-item>
                        </b-nav-item-dropdown>
                    </b-navbar-nav>
                </b-collapse>
            </b-navbar>
        </div>

        <router-view></router-view>
    </div>
</template>

<script>
    export default {
        name: 'app',
        data: function () {
            return {
                currentLanguage: { val: 'en-US', name: 'English' },
                languageList: [
                    { val: 'en-US', name: 'English', isSelected: true },
                    { val: 'de-DE', name: 'Deutsch', isSelected: false },
                    { val: 'zh-CN', name: '中文', isSelected: false }
                ],
                searchText:''
            }
        },
        computed: {
            showLanguageList() {
                let list = [];
                for (let i = 0; i < this.languageList.length; i++) {
                    if (!this.languageList[i].isSelected)
                        list.push(this.languageList[i]);
                }
                return list;
            }
        },
        methods: {
            changeLanguage(lang) {
                if (this.currentLanguage.val == lang)
                    return;
                this.currentLanguage.val = lang;
                for (let i = 0; i < this.languageList.length; i++) {
                    this.languageList[i].isSelected = (this.languageList[i].val == lang);
                    if (this.languageList[i].isSelected)
                        this.currentLanguage.name = this.languageList[i].name;
                }
                this.$loadLanguageAsync(lang);
            },
            toast(toaster, append = false) {
                const h = this.$createElement;
                const $div = h(
                    'b-input-group',
                    {
                        class: 'mt-3'
                    },
                    [
                        h('b-form-input',
                            {
                                attrs: { 'id': 'nav_search_text_input' }
                            }, ''),
                        h('b-input-group-append', {},
                            [
                                h('b-button', { on: { click: () => this.search() }}, [h('b-icon', { attrs: { icon: 'search' }})])                               
                            ])

                    ]

                );
                this.$bvToast.toast([$div], {
                    id:'nav_search_toast',
                    title: `${this.$t('search')}`,
                    toaster: toaster,
                    solid: true,
                    appendToast: append,
                    noAutoHide: true
                })
            },
            showSelectView() {
                this.toast("b-toaster-top-full", true);
            },
            search() {
                let searchText = document.getElementById('nav_search_text_input').value;
                window.console.log(searchText);
                this.$bvToast.hide('nav_search_toast');
            }
        },
        beforeMount() {
            let lang = window.localStorage.getItem('CurrentLanguage');
            this.changeLanguage(lang);
        }
    };
</script>

<style>
</style>

