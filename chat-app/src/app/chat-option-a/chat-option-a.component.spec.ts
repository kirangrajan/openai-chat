import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ChatOptionAComponent } from './chat-option-a.component';

describe('ChatOptionAComponent', () => {
  let component: ChatOptionAComponent;
  let fixture: ComponentFixture<ChatOptionAComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ChatOptionAComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ChatOptionAComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
