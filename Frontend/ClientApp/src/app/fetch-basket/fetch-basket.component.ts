import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BasketItem } from '../models/basketItem.model';
import { ApiUrls } from '../shared/ApiUrls';

@Component({
  selector: 'app-fetch-basket',
  templateUrl: './fetch-basket.component.html'
})
export class FetchBasketComponent {

  public basketItems: BasketItem[];

  private basketUrl = ApiUrls.basketApiUrl; // "https://localhost:5101/";

  constructor(private http: HttpClient) {

    http.get<BasketItem[]>(this.basketUrl + 'api/BasketItems/')
      .subscribe(result =>
      {
        console.log(result);
        this.basketItems = result;

      }, error => console.error(error));
  }

  public removeFromBasket(item: BasketItem)
  {
    this.http.delete<BasketItem>(this.basketUrl + 'api/BasketItems/' + item.id)
      .subscribe(result => {

      }, error => console.error(error));
  }
}
