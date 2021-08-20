import { Injectable, Optional } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Subject } from 'rxjs';

@Injectable()
export class ConfigLoaderService {
	private configUrl: string = './assets/config.json';
	private configObject:any;
	//public configSubject: Subject<any> = new Subject<any>();

	constructor(private _http: HttpClient) {		
	}

	loadConfig(): Promise<any> {
		return this._http
			.get(this.configUrl)
			.toPromise()
			.then((configData: any) => {
				this.configObject = configData;
				//this.configSubject.next(this.configObject);
			})
			.catch((err: any) => {
				this.configObject = null;
				//this.configSubject.next(this.configObject);
			});
	}
	getConfig() {
		return this.configObject;
	}

	getConfigObjectKey(key: string) {
		return this.configObject ? this.configObject[key] : null;
	}
}