<template>
    <div id="app">
        <div>
            <b-navbar toggleable="lg" type="dark" variant="info">
                <b-navbar-toggle target="nav-collapse"></b-navbar-toggle>
                <b-collapse id="nav-collapse" is-nav>
                    <b-navbar-nav>
                        <b-nav-item to="/">Home</b-nav-item>
                        <b-nav-item to="/room">Room</b-nav-item>
                        <b-nav-item to="/cupboard">Cupboard</b-nav-item>
                        <b-nav-item to="/thing">Thing</b-nav-item>
                    </b-navbar-nav>

                    <!-- Right aligned nav items -->
                    <b-navbar-nav class="ml-auto">
                        <b-nav-form>
                            <b-form-input size="sm" class="mr-sm-2" placeholder="Search"></b-form-input>
                            <b-button size="sm" class="my-2 my-sm-0" type="submit">Search</b-button>
                        </b-nav-form>

                        <b-nav-item-dropdown text="Lang" right>
                            <b-dropdown-item>English</b-dropdown-item>
                            <b-dropdown-item href="#">Deutsch</b-dropdown-item>                         
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
        name: 'app'
    };
</script>

<style>
</style>

