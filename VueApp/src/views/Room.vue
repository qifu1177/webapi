<template>
    <div class="container-fluid">
        <div class="row">
            <b-button-toolbar aria-label="Toolbar">
                <b-button-group size="sm" class="mx-1">
                    <b-button @click="Create()">New</b-button>
                </b-button-group>

                <b-button-group size="sm" class="mx-3">
                    <b-button>List</b-button>
                    <b-button>Card</b-button>
                </b-button-group>
            </b-button-toolbar>
        </div>
        <b-table hover :items="rooms" :fields="fields" show-empty>
            <template #cell(actions)="row">
                <b-button-group>
                    <b-button size="sm" @click="Edit(row.item, $event.target)" class="mr-1">
                        Edit
                    </b-button>
                    <b-button size="sm" @click="Delete(row.item, $event.target)">
                        Delete
                    </b-button>
                </b-button-group>
            </template>
        </b-table>
        <b-modal>
            <template #default="{ hide }">
                <p>Are you shue {{deleteRoom.Name}}</p>
            </template>
            <template #modal-footer="{ ok, cancel, hide }">
                <b>Custom Footer</b>
                <!-- Emulate built in modal footer ok and cancel button actions -->
                <b-button size="sm" variant="success" @click="DeleteOk(deleteRoom.RoomId)">
                    OK
                </b-button>
                <b-button size="sm" variant="danger" @click="cancel()">
                    Cancel
                </b-button>

            </template>
        </b-modal>
    </div>
</template>

<script>
    import roomLogic from '@/logics'
    import { CupboardLogic } from '@/logics'
    roomLogic.init('https://localhost:44356/data');
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
                    ImagePath:''
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
            this.loadRooms();
        },
    };
</script>