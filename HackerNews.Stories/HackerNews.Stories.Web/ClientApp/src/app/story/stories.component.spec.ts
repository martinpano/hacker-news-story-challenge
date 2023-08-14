import { ComponentFixture, TestBed } from '@angular/core/testing';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatInputModule } from '@angular/material/input';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatTableModule } from '@angular/material/table';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { of } from 'rxjs';
import { Story } from './models/story';
import { StoryService } from './services/story.service';

import { StoriesComponent } from './stories.component';

describe('StoriesComponent', () => {
  let component: StoriesComponent;
  let fixture: ComponentFixture<StoriesComponent>;
  let storyService: StoryService;
  const BASE_URL = "baseUrl";

  const mockStoryService = {
    getStoriesData: () =>
      of([
        { title: "Mock Story 1" } as Story,
        { title: "Mock Story 2" } as Story,
      ]),
  };
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [StoriesComponent],
      providers: [
        { provide: StoryService, useValue: mockStoryService },
        { provide: 'BASE_URL', useValue: BASE_URL }
      ],
      imports: [
        BrowserAnimationsModule,
        MatCardModule,
        MatTableModule,
        MatInputModule,
        MatPaginatorModule
      ],
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(StoriesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
    storyService = TestBed.inject(StoryService);
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
