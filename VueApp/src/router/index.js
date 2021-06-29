import vue from "vue"
import vueRouter from 'vue-router'

import Home from "@/views/Home.vue"
import Room from "@/views/Room.vue"
import RoomEdit from "@/views/RoomEdit.vue"
import Cupboard from "@/views/Cupboard.vue"
import Thing from "@/views/Thing.vue"
import Search from "@/views/Search.vue"
import Login from "@/views/Login.vue"
import Register from "@/views/Register.vue"
import Profile from "@/views/Profile.vue"

vue.use(vueRouter);
const pageNotFound = { template: '<p>page not found!</p>' }
const routes = [
    { path: '/', component: Home },
    { path: '/room', component: Room },
    { path: '/room/edit', component: RoomEdit },
    { path: '/cupboard', component: Cupboard },
    { path: '/thing', component: Thing },
    { path: '/search', component: Search },
    { path: '/user/login', component: Login },
    { path: '/user/registe', component: Register },
    { path: '/user/profile', component: Profile },
    {
        path: '*',
        component: pageNotFound,
    },
];

const route = new vueRouter({
    mode: 'history',
    base: process.env.BASE_URL,
    routes
});
export default route;