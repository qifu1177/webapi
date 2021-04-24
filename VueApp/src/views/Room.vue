<template>
    <div>
        <b-table striped hover :items="rooms"></b-table>
    </div>
</template>

<script>
    import roomLogic  from '@/logics'
    roomLogic.init('https://localhost:44356/data');
    export default {
        name: 'Room',
        data: function () {
            return {
                rooms:[]
            }
        },
        computed: {
            get_class: function () {
                return 'active';
            }
        },
        methods: {
            loadRooms: function () {               
                roomLogic.loadRooms(this.rooms);
                
            },
            
        },
        beforeMount() {
            this.loadRooms();
        },
    };
</script>