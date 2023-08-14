import { AfterViewInit, Component, Inject, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { Subscription } from 'rxjs';
import { Story } from './models/story';
import { StoryService } from './services/story.service';

@Component({
  selector: 'app-stories',
  templateUrl: './stories.component.html',
  styleUrls: ['./stories.component.css']
})
export class StoriesComponent implements OnInit, OnDestroy {
  private baseUrl!: string;
  stories!: Story[];
  sub!: Subscription;

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  displayedColumns: string[] = ['id', 'title'];
  dataSource!: MatTableDataSource<Story>;


  constructor(private storyService: StoryService, @Inject('BASE_URL') baseUrl: string) {
    this.baseUrl = baseUrl;
  }

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }

  ngOnInit(): void {
    this.sub = this.storyService.getStoriesData(this.baseUrl + 'api/story')
      .subscribe(data => {
        this.stories = data;
        this.dataSource = new MatTableDataSource<Story>(this.stories);
        this.dataSource.paginator = this.paginator;
      });
  }

  ngOnDestroy(): void {
    this.sub.unsubscribe();
  }
}
