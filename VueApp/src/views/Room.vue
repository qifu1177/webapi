<template>
    <div class="container-fluid">
        <br />
        <div>
            <b-button-group size="sm" class="mx-1">
                <b-button @click="Create()"><b-icon icon="file-earmark-richtext"></b-icon></b-button>
                <b-button @click="showList=true"><b-icon icon="list-check"></b-icon></b-button>
                <b-button @click="showList=false"><b-icon icon="grid"></b-icon></b-button>
            </b-button-group>
        </div>
        <br />
        <b-table hover :items="rooms" :fields="fields" responsive="sm" show-empty>
            <template v-slot:cell(Name)="data">
                <div variant="light">{{data.item.Name}}</div>
            </template>
            <template #cell(actions)="data">
                <b-button-group>
                    <b-button variant="light" size="sm" @click="Edit(data.item, $event.target)" class="mr-1">
                        <b-icon icon="pencil-square" variant="default"></b-icon>
                    </b-button>
                    <b-button variant="light" size="sm" @click="Delete(data.item, $event.target)">
                        <b-icon icon="trash" variant="danger"></b-icon>
                    </b-button>
                </b-button-group>
            </template>
        </b-table>

    </div>
</template>

<script>
    import roomLogic from '@/logics'
    import { CupboardLogic } from '@/logics'

    export default {
        name: 'Room',
        data: function () {
            return {
                rooms: [],
                fields: [
                    {
                        key: 'Name',
                        sortable: true
                    },
                    {
                        key: 'ImagePath',
                        sortable: false
                    },
                    {
                        key: 'TS',
                        label: 'Date',
                        sortable: true
                    },
                    { key: 'actions', label: '' }
                ],
                deleteRoom: { Name: '', RoomId: 0 },
                showList: true
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
                window.console.log(CupboardLogic);
            },
            Create: function () {
                let room = {
                    RoomId: 0,
                    Name: '',
                    Cupboards: [],
                    TS: new Date(),
                    ImagePath: ''
                };
                this.$router.push({ path: 'room/edit', query: room });
            },
            Edit: function (item, button) {
                window.console.log(button);
                this.$router.push({ path: 'room/edit', query: item });
            },
            Delete: function (item, button) {
                window.console.log(button);
                this.deleteRoom = item;

            },
            DeleteOk: function (roomId) {
                roomLogic.deleteRoom(roomId);
            }
        },
        beforeMount() {
            roomLogic.init(this.$baseServerUrl);
            roomLogic.setErrorFunc(this.$createShowMessage('error', this));
            this.loadRooms();
        },
    };
</script>