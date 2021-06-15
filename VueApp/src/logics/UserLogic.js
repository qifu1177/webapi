import CryptoJS from "crypto-js"
import ConstString from "@/const"
import logic from "@/logics/Logic"
function UserLogic() {
    this.userLogin = (input, output) => {
        let url = this.http.baseUrl + '/user/login/' + input.email;
        this.http.get(url, { headers: { psw: CryptoJS.MD5(input.psw) } }).then(function (datas) {
            output.data = datas;
            window.sessionStorage.setItem(ConstString.LgoinInfo, JSON.stringify(output.data));
        }, this.errorFunc);
    }
    this.userRegister= (input, output) => {
        let url = this.http.baseUrl + '/user/register';
        let formData = new FormData();
        for (let k in input) {
            if (k == 'psw')
                formData.append(k, CryptoJS.MD5(input[k]));
            else
                formData.append(k, input[k]);
        }
        this.http.post(url, formData).then(function (datas) {
                output.message = datas.message;
        }, this.errorFunc);
    }
}
UserLogic.prototype = logic;
export default new UserLogic();
