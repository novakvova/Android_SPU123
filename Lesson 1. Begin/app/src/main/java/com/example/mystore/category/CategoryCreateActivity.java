package com.example.mystore.category;

import android.content.Intent;
import android.database.Cursor;
import android.net.Uri;
import android.os.Bundle;
import android.provider.MediaStore;
import android.util.Log;
import android.view.View;
import android.widget.ImageView;

import com.bumptech.glide.Glide;
import com.bumptech.glide.request.RequestOptions;
import com.example.mystore.BaseActivity;
import com.example.mystore.MainActivity;
import com.example.mystore.R;
import com.example.mystore.application.HomeApplication;
import com.example.mystore.dto.CategoryCreateDTO;
import com.example.mystore.dto.CategoryItemDTO;
import com.example.mystore.service.ApplicationNetwork;
import com.google.android.material.textfield.TextInputLayout;

import okhttp3.MediaType;
import okhttp3.MultipartBody;
import okhttp3.RequestBody;
import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class CategoryCreateActivity extends BaseActivity {

    // Define a constant for image pick request
    private static final int PICK_IMAGE_REQUEST = 1;
    TextInputLayout tfCategoryName;
    ImageView ivSelectImage;
    String filePath;
    //TextInputLayout tfCategoryImage;
    TextInputLayout tfCategoryDescription;
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_category_create);

        ivSelectImage=findViewById(R.id.ivSelectImage);
        String url = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQuLnfPLVTh4KeLNvuyj15nRqSQ71A5yccavrGpwlTX2RJlM-_BD3_yCFALnCyOLHEoz1w&usqp=CAU";
        Glide
                .with(HomeApplication.getAppContext())
                .load(url)
                .apply(new RequestOptions().override(300))
                .into(ivSelectImage);

        tfCategoryName=findViewById(R.id.tfCategoryName);
        //tfCategoryImage=findViewById(R.id.tfCategoryImage);
        tfCategoryDescription=findViewById(R.id.tfCategoryDescription);
    }

    public void onClickCreateCategory(View view)
    {
        String name = tfCategoryName.getEditText().getText().toString().trim();
        //String image = tfCategoryImage.getEditText().getText().toString().trim();
        String description = tfCategoryDescription.getEditText().getText().toString().trim();

//        CategoryCreateDTO dto = new CategoryCreateDTO();

        RequestBody requestFile =
                RequestBody.create(MediaType.parse("multipart/form-data"), filePath);

//        dto.setName();
//        dto.setImage();
//        dto.setDescription();

        ApplicationNetwork.getInstance()
                .getCategoriesApi()
                .create(
                        MultipartBody.Part.createFormData("file", "salo.jpg", requestFile),
                        RequestBody.create(MediaType.parse("text/plain"), name),
                        RequestBody.create(MediaType.parse("text/plain"), description))
                .enqueue(new Callback<CategoryItemDTO>() {
                    @Override
                    public void onResponse(Call<CategoryItemDTO> call, Response<CategoryItemDTO> response) {
                        if(response.isSuccessful())
                        {
                            CategoryItemDTO result = response.body();
                            Intent intent = new Intent(CategoryCreateActivity.this, MainActivity.class);
                            startActivity(intent);
                            finish();
                        }
                    }

                    @Override
                    public void onFailure(Call<CategoryItemDTO> call, Throwable t) {

                    }
                });
    }

    // Call this method when the user wants to pick an image, for example, when a button is clicked
    public void openGallery(View view) {
        Intent intent = new Intent(Intent.ACTION_PICK, android.provider.MediaStore.Images.Media.EXTERNAL_CONTENT_URI);
        startActivityForResult(intent, PICK_IMAGE_REQUEST);
    }

    @Override
    protected void onActivityResult(int requestCode, int resultCode, Intent data) {
        super.onActivityResult(requestCode, resultCode, data);

        if (requestCode == PICK_IMAGE_REQUEST && resultCode == RESULT_OK && data != null && data.getData() != null) {
            // Get the URI of the selected image
            Uri uri = data.getData();

            Glide
                    .with(HomeApplication.getAppContext())
                    .load(uri)
                    .apply(new RequestOptions().override(300))
                    .into(ivSelectImage);

            // If you want to get the file path from the URI, you can use the following code:
            filePath = getPathFromURI(uri);
        }
    }

    // This method converts the image URI to the direct file system path of the image file
    private String getPathFromURI(Uri contentUri) {
        String[] projection = {MediaStore.Images.Media.DATA};
        Cursor cursor = getContentResolver().query(contentUri, projection, null, null, null);
        if (cursor != null) {
            int column_index = cursor.getColumnIndexOrThrow(MediaStore.Images.Media.DATA);
            cursor.moveToFirst();
            String filePath = cursor.getString(column_index);
            cursor.close();
            return filePath;
        }
        return null;
    }


}