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
        <div v-if="showList">
            <b-table hover :items="rooms" :fields="fields" responsive="sm" show-empty>
                <template v-slot:cell(Name)="data">
                    <div variant="light">{{data.item.Name}}</div>
                </template>
                <template v-slot:cell(ImagePath)="data">
                    <FileView :is-list="true" :is-img="data.item.IsImage" :name="data.item.ImagePath" :icon="data.item.Icon" :link="data.item.Link"></FileView>
                </template>
                <template v-slot:cell(TS)="data">
                    <div variant="light" v-df="{'val':data.item.TS,'f':'long'}"></div>
                </template>
                <template #cell(actions)="data">
                    <b-button-group>
                        <b-button variant="light" size="sm" @click="Edit(data.item)" class="mr-1">
                            <b-icon icon="pencil-square" variant="default"></b-icon>
                        </b-button>
                        <b-button variant="light" size="sm" @click="data.toggleDetails">
                            <b-icon icon="trash" variant="danger"></b-icon>
                        </b-button>
                    </b-button-group>
                </template>
                <template #row-details="data">
                    <b-card>
                        <div>
                            Are you sure you want to delete the data {{data.item.Name}}?
                        </div>
                        <b-button-group size="sm" class="mx-3">
                            <b-button size="sm" @click="deleteRoom(data.item)" variant="danger">OK</b-button>
                            <b-button size="sm" @click="data.toggleDetails" variant="light">Cancel</b-button>
                        </b-button-group>
                    </b-card>
                </template>
            </b-table>
        </div>
        <div v-else>
            <b-card-group columns>
                <b-card v-for="(item,index) in rooms" :key="index">
                    <b-row>
                        <b-col align="center">
                            <FileView :is-list="false" :is-img="item.IsImage" :name="item.ImagePath" :icon="item.Icon" :link="item.Link"></FileView>
                        </b-col>
                    </b-row>
                    <b-row>
                        <b-col align="center"><b>{{item.Name}}</b></b-col>
                    </b-row>
                    <b-row>
                        <b-col align="center">
                            <b-button-group>
                                <b-button variant="light" size="sm" @click="Edit(item)" class="mr-1">
                                    <b-icon icon="pencil-square" variant="default"></b-icon>
                                </b-button>
                                <b-button variant="light" size="sm" @click="item.showDelete=true">
                                    <b-icon icon="trash" variant="danger"></b-icon>
                                </b-button>
                            </b-button-group>
                        </b-col>
                    </b-row>
                    <b-row v-show="item.showDelete">
                        <b-col>
                            <div>
                                Are you sure you want to delete the data {{item.Name}} ?
                            </div>
                            <b-button-group size="sm" class="mx-3">
                                <b-button size="sm" @click="deleteRoom(item)" variant="danger">OK</b-button>
                                <b-button size="sm" @click="item.showDelete=false" variant="light">Cancel</b-button>
                            </b-button-group>
                        </b-col>
                    </b-row>
                </b-card>
            </b-card-group>
        </div>


    </div>
</template>

<script>
    import roomLogic from '@/logics'
    import fileView from '@/components/FileView.vue';
    export default {
        name: 'Room',
        components: {
            FileView: fileView
        },
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
                outputData: { message: '', exception: '' },
                showList: true
            }
        },
        computed: {            
        },
        watch: {
            outputData: {
                handler(newData) {
                    if (newData.message && newData.message == 'OK') {
                        this.outputData.message = '';
                        this.loadRooms();
                    }
                },
                deep: true
            }
        },
        methods: {
            loadRooms: function () {
                roomLogic.loadRooms(this.rooms);               
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
            Edit: function (item) {
                this.$router.push({ path: 'room/edit', query: item });
            },
            deleteRoom: function (item) {
                this.outputData.message = '';
                roomLogic.deleteRoom(item.RoomId, this.outputData);
            },
            openFile(filename) {
                window.open(this.$baseServerUrl + '/file/' + filename, '_blank');
            }
        },
        beforeMount() {
            roomLogic.init(this.$baseServerUrl);
            roomLogic.setErrorFunc(this.$createShowMessage('error', this));
            this.loadRooms();
        },
    };
</script>