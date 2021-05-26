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
                            <b-form-input size="sm" class="mr-sm-2" placeholder="Search"></b-form-input>
                            <b-button size="sm" class="my-2 my-sm-0" type="submit">Search</b-button>
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
                ]
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

