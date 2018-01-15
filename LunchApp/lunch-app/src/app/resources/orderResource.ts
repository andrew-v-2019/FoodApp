import { Injectable } from '@angular/core';
import { Resource, ResourceParams, ResourceAction, ResourceMethod } from 'ngx-resource';
import { RequestMethod } from '@angular/http';
import { environment } from 'environments/environment';


@Injectable()
@ResourceParams({
    url: environment.apiEndpoint + '/orders/'
})

export class LunchResource extends Resource {
    
}

