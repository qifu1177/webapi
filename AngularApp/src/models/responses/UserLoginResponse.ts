import {SessionResponse} from "./SessionResponse";
export interface UserLoginResponse extends  SessionResponse{
    UserName:string;
    ModuleRights:any;
    SessionDuration:number;
    UploadFileMaxSize:number;
    UploadFileTypes:string[];
}