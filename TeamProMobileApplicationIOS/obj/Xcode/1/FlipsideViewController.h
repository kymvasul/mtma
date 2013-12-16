// WARNING
// This file has been generated automatically by Xamarin Studio to
// mirror C# types. Changes in this file made by drag-connecting
// from the UI designer will be synchronized back to C#, but
// more complex manual changes may not transfer correctly.


#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>


@interface FlipsideViewController : UIViewController {
	UILabel *_loginLabel;
	UITextField *_textBoxLogin;
	UITextField *_textBoxPassword;
}

@property (nonatomic, retain) IBOutlet UILabel *loginLabel;

@property (nonatomic, retain) IBOutlet UITextField *textBoxLogin;

@property (nonatomic, retain) IBOutlet UITextField *textBoxPassword;

- (IBAction)done:(UIBarButtonItem *)sender;

- (IBAction)ckick:(id)sender;

- (IBAction)loginClick:(id)sender;

@end
