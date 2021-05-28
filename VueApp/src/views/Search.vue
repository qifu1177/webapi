<template>
    <div>{{searchText}}</div>
</template>

<script>
    export default {
        name: 'Search',
        data: function () {
            return {
                searchText:''
            }
        },
        mounted: function () {
            this.searchText = this.$router.currentRoute.query.text;
            
        }
    };
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
</style>