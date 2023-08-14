import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { StoryService } from './story.service';
import { Story } from '../models/story';

describe('StoryService', () => {
  let service: StoryService;
  let httpTestingController: HttpTestingController;
  const BASE_URL = "baseUrl";

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [StoryService, {provide: 'BASE_URL', useValue: BASE_URL}]
    }).compileComponents();
    
    service = TestBed.inject(StoryService);
    httpTestingController = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpTestingController.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });



  it('should retrieve stories data', () => {
    const testStories: Story[] = [
      { id: 1, title: 'Story 1', url: 'https://dummy-api/story1' },
      { id: 2, title: 'Story 2', url: 'https://dummy-api/story1' }
    ];
    const url = 'http://localhost:5600/story';

    service.getStoriesData(url).subscribe((stories: Story[]) => {
      expect(stories).toEqual(testStories);
    });

    const req = httpTestingController.expectOne(url);
    expect(req.request.method).toEqual('GET');
    req.flush(testStories);
  });
});
