import { createNamespacedHelpers } from 'vuex';
const { mapGetters } = createNamespacedHelpers('authenticate');
function createBaseViewObj(obj) {
    
    let baseVueObj = {
        computed: {
            ...mapGetters({
                SessionId: "SessionId"
            }),
        },
        beforeCreate() {
            this.$base.loginCheck();
        }
    };
    function mergeComputed(obj, baseObj) {
        if (typeof obj.computed === 'object') {
            for (let item in baseObj.computed) {
                if (typeof obj.computed[item] === 'undefined')
                    obj.computed[item] = baseObj.computed[item];
            }
        } else {
            obj.computed = baseObj.computed;
        }
    }
    function mergeBeforeCreate(obj, baseObj) {
        if (typeof obj.beforeCreate === 'function') {
            obj.beforeCreate = function () {
                baseObj.beforeCreate();
                obj.beforeCreate();
            }
        } else {
            obj.beforeCreate = baseObj.beforeCreate;
        }
    }
    mergeComputed(obj, baseVueObj);
    mergeBeforeCreate(obj, baseVueObj);
    return obj;
}
export default createBaseViewObj;