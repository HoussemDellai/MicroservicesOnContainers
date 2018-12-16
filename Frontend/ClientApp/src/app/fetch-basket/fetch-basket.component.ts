import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-fetch-basket',
  templateUrl: './fetch-basket.component.html'
})
export class FetchBasketComponent {

  public basketItems: BasketItem[];

  private basketUrl = "https://localhost:5101/";

  constructor(http: HttpClient) {

    http.get<BasketItem[]>(this.basketUrl + 'api/BasketItems/')
      .subscribe(result =>
      {
        console.log(result);
        this.basketItems = result;

      }, error => console.error(error));
  }
}

interface BasketItem {
  id: number;
  productId: number;
  name: string;
  //price: number;
}
