import { State } from "projects/store";
import { UserLoginResponse } from "src/models/responses/UserLoginResponse";
import {SessionResponse} from 'src/models/responses/SessionResponse';
export class UserState extends State<UserLoginResponse>{

    constructor() {
        super();
        this.data = { userName: "", sessionId: "", sessionDuration: 0, sessionUpdateTs: 0, moduleRights: {}, uploadFileMaxSize: 0, uploadFileTypes: [] };
        this.createAction("login", this.login);
        this.createAction("logout", this.logout);
        this.createFunc("checkLogin", this.checkLogin);
        this.createAction("updateSession",this.updateSession);
    }
    
    login(user: UserLoginResponse, args:any) {
        let loginUser:UserLoginResponse=args[0];
        user.userName = loginUser.userName;
        user.sessionId = loginUser.sessionId;
        user.sessionDuration = loginUser.sessionDuration;
        user.sessionUpdateTs = loginUser.sessionUpdateTs;
        user.moduleRights = loginUser.moduleRights;
        user.uploadFileMaxSize = loginUser.uploadFileMaxSize;
        user.uploadFileTypes = loginUser.uploadFileTypes;
    }
    logout(user: UserLoginResponse) {
        user.userName = "";
        user.sessionId = "";
        user.sessionDuration = 0;
        user.sessionUpdateTs = 0;
        user.moduleRights = {};
        user.uploadFileMaxSize = 0;
        user.uploadFileTypes = [];
    }
    checkLogin(user: UserLoginResponse): boolean {
        let b: boolean = false;
        b = user.sessionId != "";
        if (b)
            b = ((new Date()).getTime() - user.sessionUpdateTs) < user.sessionDuration * 1000;
        return b;
    }
    updateSession(user: UserLoginResponse,args:any){
        let session:SessionResponse=args[0];
        user.sessionId=session.sessionId;
        user.sessionUpdateTs=session.sessionUpdateTs;
    }
}