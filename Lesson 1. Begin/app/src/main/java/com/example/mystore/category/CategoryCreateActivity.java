package com.example.mystore.category;

import android.content.Intent;
import android.os.Bundle;
import android.view.View;

import com.example.mystore.BaseActivity;
import com.example.mystore.MainActivity;
import com.example.mystore.R;
import com.example.mystore.dto.CategoryCreateDTO;
import com.example.mystore.dto.CategoryItemDTO;
import com.example.mystore.service.ApplicationNetwork;
import com.google.android.material.textfield.TextInputLayout;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class CategoryCreateActivity extends BaseActivity {

    TextInputLayout tfCategoryName;
    TextInputLayout tfCategoryImage;
    TextInputLayout tfCategoryDescription;
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_category_create);

        tfCategoryName=findViewById(R.id.tfCategoryName);
        tfCategoryImage=findViewById(R.id.tfCategoryImage);
        tfCategoryDescription=findViewById(R.id.tfCategoryDescription);
    }

    public void onClickCreateCategory(View view)
    {
        String name = tfCategoryName.getEditText().getText().toString().trim();
        String image = tfCategoryImage.getEditText().getText().toString().trim();
        String description = tfCategoryDescription.getEditText().getText().toString().trim();

        CategoryCreateDTO dto = new CategoryCreateDTO();
        dto.setName(name);
        dto.setImage(image);
        dto.setDescription(description);
        ApplicationNetwork.getInstance()
                .getCategoriesApi()
                .create(dto)
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
}