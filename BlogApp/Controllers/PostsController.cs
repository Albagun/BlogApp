using System.Security.Claims;
using BlogApp.Data.Abstract;
using BlogApp.Data.Concrete.EfCore;
using BlogApp.Entity;
using BlogApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace BlogApp.Controllers{

   public class PostsController : Controller
   {
     private IPostRepository _postrepository;
      private ICommentRepository _commentrepository;
      private ITagRepository _tagrepository;

     public PostsController(IPostRepository postRepository, ICommentRepository commentRepository, ITagRepository tagRepository)
     {
        _postrepository = postRepository;
        _commentrepository =commentRepository;
        _tagrepository =tagRepository;
     }

     public async Task<IActionResult> Index(string tag)
     {   
        
         var posts = _postrepository.Posts.Where(i=> i.IsActive);
         if (!string.IsNullOrEmpty(tag))
         {
                  posts = posts.Where(x => x.Tags.Any(t => t.Url ==tag));

         }
         return View(
          new PostsViewModel
          {
                Posts= await posts.ToListAsync()
          }
          );
     }   
     public async Task<IActionResult> Details(string url) 
     {
          
          return View(await _postrepository
          .Posts
          .Include(x =>x.User)
          .Include(x => x.Tags)
          .Include(x => x.Comments)
          .ThenInclude(x => x.User)
         .FirstOrDefaultAsync(p=>p.Url == url));

     }
      
     [HttpPost]
     public JsonResult AddComment(int PostId, string Text)
     {  
        var userId = User.FindFirstValue (ClaimTypes.NameIdentifier);
        var username = User.FindFirstValue (ClaimTypes.Name);
        var avatar= User.FindFirstValue(ClaimTypes.UserData);

        var entity= new Comment {
         PostId=PostId,
         Text = Text,
         PublishedOn = DateTime.Now,
         
         UserId = int.Parse(userId ?? "")
        };
        _commentrepository.CreateComment(entity);

      // return Redirect("/posts/details/"+ Url);

      return Json(new {
            username,
            Text,
            entity.PublishedOn,
            avatar
           
      });
        

     }


         [Authorize]
         public IActionResult Create()
         {
          return View();
         }
             
        [HttpPost]
         [Authorize]
           public IActionResult Create(PostCreateViewModel model)
         {

        var userId = User.FindFirstValue (ClaimTypes.NameIdentifier);
          if(ModelState.IsValid){
  
            _postrepository.CreatePost(
              new Post {

                Title = model.Title,
                Content =model.Content,
                Url= model.Url,
                UserId = int.Parse(userId ?? ""),
                PublishedOn = DateTime.Now,
                Image = "1.jpg",
                IsActive = false
              }
            );

            return RedirectToAction("Index");

          }
          return View(model);
         }
      
      [Authorize]
     public async Task<IActionResult> List()
     {

        var userId = int.Parse(User.FindFirstValue (ClaimTypes.NameIdentifier) ?? "");
        var role = User.FindFirstValue (ClaimTypes.Role);

        var posts= _postrepository.Posts;
        
        if(string.IsNullOrEmpty(role))
        {

            posts = posts.Where(i => i.UserId == userId);

        }

        return View(await posts.ToListAsync());
        

     }
   
       [Authorize]
     public IActionResult Edit(int? id)
      {
            if(id==null){

               return NotFound();

            }
            var post= _postrepository.Posts.Include(i=>i.Tags).FirstOrDefault(i => i.PostId== id);
            if(post==null)
            {
               return NotFound();
            }

            ViewBag.Tags= _tagrepository.Tags.ToList();

            return View(new PostCreateViewModel{

                PostId= post.PostId,
                Title= post.Title,
                Description= post.Description,
                Content= post.Content,
                Url= post.Url,
                IsActive = post.IsActive,
                Tags= post.Tags

            });
      
      }


      [Authorize]
      [HttpPost]
     public IActionResult Edit(PostCreateViewModel model, int[] tagIds){

      if(ModelState.IsValid){
        var entityToUpdate= new Post {
          PostId =model.PostId,
          Title= model.Title,
          Description= model.Description,
          Content=model.Content,
          Url= model.Url

        };

          if(User.FindFirstValue(ClaimTypes.Role)=="admin")
          {

                entityToUpdate.IsActive = model.IsActive;
 
          }


        _postrepository.EditPost(entityToUpdate, tagIds);
        return RedirectToAction("List");
      }
             
              ViewBag.Tags= _tagrepository.Tags.ToList();
             return View(model);
     }

    [Authorize]
    public async Task<IActionResult> Delete(int? id)
{
    if (id == null)
    {
        return NotFound();
    }

    var postToDelete = await _postrepository.Posts.FirstOrDefaultAsync(p => p.PostId == id);
    if (postToDelete == null)
    {
        return NotFound();
    }

    return View(postToDelete);
}

[HttpPost, ActionName("Delete")]
[Authorize]
public IActionResult DeleteConfirmed(int id)
{
    var postToDelete = _postrepository.Posts.FirstOrDefault(p => p.PostId == id);
    if (postToDelete == null)
    {
        return NotFound();
    }

    _postrepository.DeletePost(postToDelete); // Replace this line with the actual method to delete the post
    return RedirectToAction("List"); // Redirect to the list page or any other page after successful deletion
}

[Authorize]
public async Task<IActionResult> DeleteComment(int? commentId)
{
    if (commentId == null)
    {
        return NotFound();
    }

    var commentToDelete = await _commentrepository.Comments.FirstOrDefaultAsync(c => c.CommentId == commentId);
    if (commentToDelete == null)
    {
        return NotFound();
    }

    return View(commentToDelete);
}

[HttpPost, ActionName("DeleteComment")]
[Authorize]
public IActionResult DeleteCommentConfirmed(int commentId)
{
    var commentToDelete = _commentrepository.Comments.FirstOrDefault(c => c.CommentId == commentId);
    if (commentToDelete == null)
    {
        return NotFound();
    }

    _commentrepository.DeleteComment(commentId);
    return RedirectToAction("Details", new { url = commentToDelete.Post.Url }); // Redirect to the details page after successful deletion
}
    }

   }



