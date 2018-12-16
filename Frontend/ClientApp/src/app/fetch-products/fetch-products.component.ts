import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Product } from '../models/product.model';
import { BasketItem } from '../models/basketItem.model';

@Component({
  selector: 'app-fetch-products',
  templateUrl: './fetch-products.component.html'
})
export class FetchProductsComponent {

  public products: Product[];

  private catalogUrl = ApiUrls.catalogApiUrl; // "https://localhost:5001/";

  private basketUrl = ApiUrls.basketApiUrl; // "https://localhost:44387/";

  constructor(private http: HttpClient) {

    http.get<Product[]>(this.catalogUrl + 'api/Products/')
      .subscribe(result =>
      {
        console.log(result);
        this.products = result;
      }, error => console.error(error));
  }

  public addToBasket()
  {
    basketItem: BasketItem = new BasketItem
    {

    }

    this.http.post<BasketItem>(this.basketUrl + 'api/BasketItems/', )
      .subscribe(result =>
      {
        
      }, error => console.error(error));
  }
}
