<template>
    <div class="home" v-t="'home'">
        
    </div>
</template>

<script>
    export default {
        name: 'Home',
        beforeCreate() {            
            this.$base.loginCheck();
        }
    };
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
</style>

