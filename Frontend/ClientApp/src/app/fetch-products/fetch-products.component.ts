import { Component, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Product } from '../models/product.model';
import { BasketItem } from '../models/basketItem.model';
import { ApiUrls } from '../shared/ApiUrls';

@Component({
  selector: 'app-fetch-products',
  templateUrl: './fetch-products.component.html'
})
export class FetchProductsComponent {

  public products: Product[];

  private catalogUrl = ApiUrls.catalogApiUrl; // "https://localhost:5001/";

  private basketUrl = ApiUrls.basketApiUrl; // "https://localhost:44387/";

  httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json'
    })
  };

  constructor(private http: HttpClient) {

    http.get<Product[]>(this.catalogUrl + 'api/Products/')
      .subscribe(result =>
      {
        console.log(result);
        this.products = result;
      }, error => console.error(error));
  }

  public addToBasket(product: Product)
  {
    let basketItem = new BasketItem();
    basketItem.productId = product.id;
    basketItem.name = product.name;

    this.http.post<BasketItem>(this.basketUrl + 'api/BasketItems/', basketItem, this.httpOptions)
      .subscribe(result =>
      {
        
      }, error => console.error(error));
  }
}
