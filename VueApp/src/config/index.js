import http from "@/services/http"
let baseServerUrl = 'https://localhost:44356';
let defaultData = {
    uploadFile: {
        type: ["jpg", "gif"],
        maxSize: 5
    },
    session: {
        duration: 300
    }
}
function createConfigLoad() {
    return {
        load: async function (obj) {            
            await http.get(baseServerUrl + '/user/config', {}).then(res => {
                if (res) {
                    defaultData = res;
                }              
                defaultData['baseServerUrl'] = baseServerUrl;
                obj.$config = defaultData;                
            }, (ex) => window.alert(ex));
        }
    }
}

export default createConfigLoad();