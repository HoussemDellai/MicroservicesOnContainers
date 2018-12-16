import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-fetch-products',
  templateUrl: './fetch-products.component.html'
})
export class FetchProductsComponent {

  public products: Product[];

  private catalogUrl = "https://localhost:5001/";

  private basketUrl = "https://localhost:44387/";

  constructor(private http: HttpClient) {

    http.get<Product[]>(this.catalogUrl + 'api/Products/')
      .subscribe(result =>
      {
        console.log(result);
        this.products = result;

      }, error => console.error(error));
  }

  public addToBasket() {

    this.http.get<Product[]>(this.catalogUrl + 'api/Products/')
      .subscribe(result => {
        console.log(result);
        this.products = result;

      }, error => console.error(error));
  }
}

interface Product {
  id: number;
  name: string;
  price: number;
}
