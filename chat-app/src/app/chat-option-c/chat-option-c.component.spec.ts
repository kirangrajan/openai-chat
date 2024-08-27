import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ChatOptionCComponent } from './chat-option-c.component';

describe('ChatOptionCComponent', () => {
  let component: ChatOptionCComponent;
  let fixture: ComponentFixture<ChatOptionCComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ChatOptionCComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ChatOptionCComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
