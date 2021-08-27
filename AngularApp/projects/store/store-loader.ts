import { Action, Func } from "./store-delegate";
import { State } from "./store-abstract";

export class Store {
    private static states: Map<string, any> = new Map();
    private static actions: Map<string, Action<any>> = new Map();
    private static funcs: Map<string, Func<any>> = new Map();
    static create<T>(key: string, state: State<T>) {
        this.states.set(key, state.data);
        state.actions.forEach((action, actionKey) => {
            this.actions.set(`${key}-${actionKey}`, action);
        });
        state.funcs.forEach((func, funcKey) => {
            this.funcs.set(`${key}-${funcKey}`, func);
        })
    }
    static get(key: string): any {
        return this.states.get(key);
    }
    static action(key: string, actionKey: string) {
        let newKey = `${key}-${actionKey}`;
        let currentAction: Action<any> | undefined = this.actions.get(newKey);
        let state = this.get(key);

        return (...args: any) => {
            if (typeof currentAction != 'undefined')
                currentAction(state, args);
        }
    }
    static func(key: string, funcKey: string) {
        let newKey = `${key}-${funcKey}`;
        let currentFunc: Func<any> | undefined = this.funcs.get(newKey);
        let state = this.get(key);

        return (...args: any) => {
            if (typeof currentFunc != 'undefined')
                return currentFunc(state, args);
        }
    }
}