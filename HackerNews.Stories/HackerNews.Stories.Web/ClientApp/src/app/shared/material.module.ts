import { NgModule } from '@angular/core';

import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatTableModule } from '@angular/material/table';
import { MatInputModule } from '@angular/material/input';
import { MatPaginatorModule } from '@angular/material/paginator';

@NgModule({
  declarations: [],
  exports: [
    MatCardModule,
    MatButtonModule,
    MatTableModule,
    MatInputModule,
    MatPaginatorModule
  ]
})
export class MaterialModule { }
