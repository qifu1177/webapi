import http from "@/services/http"
function HttpData() {
    this.baseUrl = '';
    this.init = (baseUrl) => this.baseUrl = baseUrl + '/data';
    this.get = (url) => { return http.get(url); }
    this.all = (className, sessionId) => {
        let url = `${this.baseUrl}/all/${className}/${sessionId}`;
        return http.get(url);
    };
    this.listWithParentId = (className, parentId) => {
        let url = `${this.baseUrl}/list/id/${className}/${parentId}`;
        return http.get(url);
    };
    this.id = (className, id) => {
        let url = `${this.baseUrl}/id/${className}/${id}`;
        return http.get(url);
    };
    this.createFormData = (sessionid, inputData) => {
        let formData = new FormData();
        formData.append("SessionId", sessionid);
        for (let k in inputData) {
            formData.append(k, inputData[k]);
        }
        return formData;
    };
    this.post = (className, sessionid, inputData) => {
        let url = `${this.baseUrl}/${className}`;
        let formdata = this.createFormData(sessionid, inputData);
        return http.post(url, formdata);
    };
    this.put = (className, sessionid, inputData) => {
        let url = `${this.baseUrl}/${className}`;
        let formdata = this.createFormData(sessionid, inputData);
        return http.put(url, formdata);
    };
    this.deleteWithId = (className, id) => {
        let url = `${this.baseUrl}/${className}/${id}`;
        return http.delete(url);
    }
}
export default new HttpData();
