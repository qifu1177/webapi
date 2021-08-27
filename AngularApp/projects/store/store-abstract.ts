import { Action,Func } from "./store-delegate";
export abstract class State<T> {
    data!:T;
    actions:Map<string,Action<T>>=new Map();
    funcs:Map<string,Func<T>>=new Map();
    createAction(key:string,action:Action<T>){
        this.actions.set(key,action);
    }
    createFunc(key:string,func:Func<T>){
        this.funcs.set(key,func);
    }
}