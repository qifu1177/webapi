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
                                <b-form-input placeholder="Search" @focus="showSearch()"></b-form-input>
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
        <b-alert v-model="showSearchDiv"
                 class="position-fixed fixed-top m-0 rounded-0"
                 style="z-index: 2000;"
                 variant="success"
                 dismissible>
            <b-input-group class='mt-3'>
                <b-form-input ref="navbarSearchTextInput" :type="'search'" v-model="searchText"></b-form-input>
                <b-input-group-append>
                    <b-button @click="search()">
                        <b-icon icon="search"></b-icon>
                    </b-button>
                </b-input-group-append>
            </b-input-group>
        </b-alert>
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
                showSearchDiv: false,
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
            search() {               
                window.console.log(this.searchText);
                this.showSearchDiv = false;
                this.$router.push({ path: 'search', query: { text: this.searchText } });
            },
            showSearch() {
                this.showSearchDiv = true;
                let that = this;
                window.setTimeout(function () {                    
                    let input = that.$refs.navbarSearchTextInput;
                    input.focus();
                }, 50);
                
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

