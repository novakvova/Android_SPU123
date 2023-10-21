package com.example.mystore.application.network;

import com.example.mystore.application.dto.category.CategoryCreateDTO;
import com.example.mystore.application.dto.category.CategoryItemDTO;

import retrofit2.Call;
import retrofit2.http.Body;
import retrofit2.http.POST;

public interface CategoriesApi {
    @POST("/api/categories/create")
    public Call<CategoryItemDTO> create(@Body CategoryCreateDTO model);
}
