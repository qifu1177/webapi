<template>
    <div @click="openFile()" variant="light" style="cursor:pointer;" v-if="isList">{{name}}</div>
    <img v-bind:src="icon" v-bind:alt="name" class="img-small" @click="openFile()" v-else/>
</template>
<script>
    export default {
        name: 'FileView',
        props: {
            icon: String,
            link: String,
            isImg: Boolean,
            name: String,            
            isList: Boolean
        },
        methods: {
            toast( toaster, append = false) {
                const h = this.$createElement;
                const $img = h(
                    'div',
                    {
                        class: 'div-img'
                    },
                    [
                        h('b-img',
                            {
                                class: 'img-fluid',
                                attrs: { src: this.link, alt: this.name }
                            }, '')
                    ]

                );
                this.$bvToast.toast([$img], {
                    title: `${this.name}`,
                    toaster: toaster,
                    solid: true,
                    appendToast: append,
                    noAutoHide: true
                })
            },
            openFile() {
                if (this.isImg) {                   
                    this.toast( "b-toaster-top-full", true);
                } else
                    window.open(this.link, "_blank");
            }
        }
    }
</script>